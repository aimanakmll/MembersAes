-- Insert a record into the Members table
INSERT INTO Members (Name, Type, Address)
VALUES ('Ali', 'Regular', '123 Main St');

-- Encrypt the original password
DECLARE @OriginalPassword NVARCHAR(MAX);
SET @OriginalPassword = 'alihensem';

DECLARE @EncryptedPassword VARBINARY(MAX);
SET @EncryptedPassword = (SELECT ENCRYPTBYPASSPHRASE('secretKey', @OriginalPassword));

-- Insert a record into the Encrypt table
INSERT INTO Encrypt (Username, EncryptedPassword, OriginalPassword)
VALUES ('Ali', @EncryptedPassword, @OriginalPassword);
