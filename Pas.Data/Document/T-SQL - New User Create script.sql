-- select NEWID ( )  
DECLARE @newuserName VARCHAR(30) = 'newtech@pas.com';
DECLARE @phoneNumber VARCHAR(30) = '0123-456789123';

INSERT INTO [dbo].[AspNetUsers]
    (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount	)
SELECT 
    NEWID(), 
	@newuserName, @newuserName, @newuserName, @newuserName
	, 1, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
FROM 
    [dbo].[AspNetUsers]

WHERE [dbo].[AspNetUsers].UserName='shakeosm@gmail.com';


