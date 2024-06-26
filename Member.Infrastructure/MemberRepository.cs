﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Member.Application;
using Member.Domain;

namespace Member.Infrastructure
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _connectionString;
        private readonly IEncryptService _encryptService;

        public MemberRepository(string connectionString, IEncryptService encryptService)
        {
            _connectionString = connectionString;
            _encryptService = encryptService;
        }

        public List<Domain.Member> GetAllMembers()
        {
            List<Domain.Member> members = new List<Domain.Member>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = @"
                SELECT Id, Name, Password
                FROM Members";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Domain.Member member = new Domain.Member
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };

                            string encryptedPassword = reader.GetString(2);

                            // Decrypt the password
                            try
                            {
                                string decryptedPassword = _encryptService.DecryptPassword(encryptedPassword);
                                member.Password = decryptedPassword; // Assign decrypted password to Password property
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error decrypting password: {ex.Message}");
                                member.Password = "Decryption error";
                            }

                            members.Add(member);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving members: {ex.Message}");
                throw;
            }

            return members;
        }

        public Domain.Member AddMember(Domain.Member member)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string encryptedPassword = _encryptService.EncryptPassword(member.Password);

                    string insertMemberQuery = @"
                        INSERT INTO Members (Name, Password) 
                        VALUES (@Name, @Password);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertMemberQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", member.Name);
                        command.Parameters.AddWithValue("@Password", encryptedPassword);

                        int memberId = Convert.ToInt32(command.ExecuteScalar());
                        member.Id = memberId;
                    }
                }

                return member;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding member: {ex.Message}");
                throw;
            }
        }
    }
}
