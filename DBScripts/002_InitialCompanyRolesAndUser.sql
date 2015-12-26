INSERT INTO CrossCountry.dbo.Organization
VALUES('KGB', 'The Committee', NULL, 'Slink, Walk, Jog, Run... We Watch. We Know', GETDATE(), 1, NULL, NULL)

INSERT INTO CrossCountry.dbo.Role
VALUES
('Admin'),('Coach'),('Athlete')

INSERT INTO CrossCountry.dbo.VarsityLevel
VALUES ('Varsity'),('Junior Varsity')

INSERT INTO CrossCountry.dbo.RunnerClassification
VALUES ('Freshman'),('Sophomore'),('Junior'), ('Senior'), ('Senorita'), ('Weekend Warrior')

INSERT INTO CrossCountry.dbo.User
VALUES (1,1,'nklassen', 'password', 'Niels', 'Klassen', NULL, NULL, NULL,GETDATE(), 1, NULL, NULL, 1)