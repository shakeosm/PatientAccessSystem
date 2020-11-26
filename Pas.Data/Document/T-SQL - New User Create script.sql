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


-- Address in  Address Book.
insert into    
	[roogi-test].[dbo].[AddressBook] 
		(UserId, AddressLine1, LocalArea, CityId, DateCreated)
select U.Id, '-', '', 10, GETDATE()
from [User].[User] AS U

--## Brands Drugs
INSERT INTO    
	[roogi-test].[Drug].[DrugBrands] 
		(DrugId, BrandName, ManufacturerId, DateAdded)
SELECT D.Id, D.Name, 1, GETDATE()
FROM [Drug].[Drugs]D



