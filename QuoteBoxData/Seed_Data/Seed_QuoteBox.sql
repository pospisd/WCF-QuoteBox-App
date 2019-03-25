--==================================================================
--
--	SAMPLE DATABASE WITH DATA:
--
--	First create a database called "QuoteBox", then run this script.
--
--==================================================================
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'QuoteBox')
	CREATE DATABASE QuoteBox
GO

USE QuoteBox;
GO

--==================================================================
--	Insert tables here...
--==================================================================
IF OBJECT_ID(N'dbo.Authors') IS NOT NULL
BEGIN;

	-- Drop any foreign key constraints.
	IF EXISTS (SELECT * FROM sys.foreign_keys WHERE referenced_object_id = OBJECT_ID(N'dbo.Authors'))
	BEGIN

		DECLARE	@SQLString nvarchar(512);

		SET @SQLString =
			(
				SELECT
						'ALTER TABLE [' + OBJECT_SCHEMA_NAME(parent_object_id) +
						'].[' + OBJECT_NAME(parent_object_id) +
						'] DROP CONSTRAINT [' + [name] + ']'
				FROM	
						sys.foreign_keys
				WHERE	
					referenced_object_id = OBJECT_ID(N'Authors')
			);

			EXECUTE sp_executesql @SQLString;

	END;

	-- Drop the table.
	DROP TABLE dbo.Authors

END;
GO

CREATE TABLE [dbo].Authors
(
	[AuthorId]				int IDENTITY(1, 1) NOT NULL,
	[FirstName]				nvarchar(256) NOT NULL,
	[MiddleName]			nvarchar(256) NULL,
	[LastName]				nvarchar(256) NOT NULL,
	[AdditionalTxt]			nvarchar(MAX) NULL,
	[CreatedDtm]			DATETIME NOT NULL,
	[VersionNum]			int NOT NULL,
	PRIMARY KEY CLUSTERED ([AuthorId] ASC)
);
GO

IF OBJECT_ID(N'dbo.Quotes') IS NOT NULL
	DROP TABLE dbo.Quotes;
GO

CREATE TABLE [dbo].Quotes
(
	[QuoteId]				int IDENTITY(1, 1) NOT NULL,
	[AuthorId]				INT NOT NULL,
	[QuoteTxt]				nvarchar(MAX) NOT NULL,
	[CreatedDtm]			DATETIME NOT NULL,
	[VersionNum]			int NOT NULL,
	PRIMARY KEY CLUSTERED ([QuoteId] ASC),
	CONSTRAINT [FK_Quotes_Authors] FOREIGN KEY ([AuthorId])
		REFERENCES [dbo].[Authors] ([AuthorId])
);
GO


--==================================================================
--	Insert procedures here...
--==================================================================
IF OBJECT_ID(N'dbo.udsp_ReadQuote') IS NOT NULL
	DROP PROCEDURE dbo.udsp_ReadQuote;
GO

CREATE PROCEDURE udsp_ReadQuote
@QuoteId		int
AS
SET NOCOUNT ON;
SELECT	q.QuoteId,
		q.AuthorId,
		a.FirstName,
		a.MiddleName,
		a.LastName,
		a.AdditionalTxt,
		a.CreatedDtm AS [AuthorCreatedDtm],
		q.QuoteTxt,
		q.CreatedDtm AS [QuoteCreatedDtm]
FROM	Quotes q
JOIN	Authors a
			ON	a.AuthorId = q.AuthorId
WHERE	QuoteId = @QuoteId;

RETURN
GO


--==================================================================
--	Insert functions here...
--==================================================================
IF OBJECT_ID(N'dbo.fn_SplitIntegerList') IS NOT NULL
	DROP FUNCTION dbo.fn_SplitIntegerList;
GO

CREATE FUNCTION [dbo].[fn_SplitIntegerList] (@IntegerList varchar(8000))
RETURNS 
@ParsedList TABLE
(
	IntegerId int 
)
AS
BEGIN
	DECLARE @IntegerId varchar(10), @Pos int
	IF @IntegerList IS NOT NULL
	BEGIN

		SET @IntegerList = LTRIM(RTRIM(@IntegerList))+ ','
		SET @Pos = CHARINDEX(',', @IntegerList, 1)
	
		IF REPLACE(@IntegerList, ',', '') <> ''
		BEGIN
			WHILE @Pos > 0
			BEGIN
				SET @IntegerId = LTRIM(RTRIM(LEFT(@IntegerList, @Pos - 1)))
				IF @IntegerId <> ''
				BEGIN
					INSERT INTO @ParsedList (IntegerId) 
					VALUES (CAST(@IntegerId AS int)) --Use Appropriate conversion
				END
				SET @IntegerList = RIGHT(@IntegerList, LEN(@IntegerList) - @Pos)
				SET @Pos = CHARINDEX(',', @IntegerList, 1)
	
			END
		END	
	END
	RETURN
END
GO




--==================================================================
--	Insert test data here...
--==================================================================

-- Authors.
SET IDENTITY_INSERT dbo.Authors ON;
GO

INSERT INTO Authors (AuthorId, FirstName, MiddleName,	LastName, AdditionalTxt, CreatedDtm, VersionNum)
VALUES (1, N'Paul', N'K.', N'Chappel', N'Army Captain, peace activist, writer.', CURRENT_TIMESTAMP, 1),
(2, N'Mark', null, N'Twain', null, CURRENT_TIMESTAMP, 1),
(3, N'Alan', null, N'Moore', null, CURRENT_TIMESTAMP, 1),
(4, N'Mahatma', null, N'Gandhi', null, CURRENT_TIMESTAMP, 1),
(5, N'Jose', null, N'Marti', null, CURRENT_TIMESTAMP, 1),
(6, N'Winston', N'S.', N'Churchill', N'Zio-Shill', CURRENT_TIMESTAMP, 1),
(7, N'Suzanne', null, N'Collins', N'Hunger Games Trilogy', CURRENT_TIMESTAMP, 1),
(8, N'Malcolm', null, N'X', null, CURRENT_TIMESTAMP, 1),
(9, N'Douglas', null, N'Adams', N'So Long, and Thanks for All the Fish', CURRENT_TIMESTAMP, 1),
(10, N'Adam', null, N'Smith', null, CURRENT_TIMESTAMP, 1),
(11, N'Elmer', N'T.', N'Peterson', null, CURRENT_TIMESTAMP, 1),
(12, N'George', null, N'Washington', null, CURRENT_TIMESTAMP, 1),
(13, N'H.', N'L.', N'Mencken', null, CURRENT_TIMESTAMP, 1),
(14, N'Theodore', null, N'Roosevelt', null, CURRENT_TIMESTAMP, 1),
(15, N'Noam', null, N'Chomsky', null, CURRENT_TIMESTAMP, 1),
(16, N'Franklin', N'D.', N'Roosevelt', null, CURRENT_TIMESTAMP, 1),
(17, N'Edward', null, N'Abbey', null, CURRENT_TIMESTAMP, 1),
(18, N'Robin', null, N'Williams', null, CURRENT_TIMESTAMP, 1),
(19, N'Martin', N'Luther', N'King', null, CURRENT_TIMESTAMP, 1),
(21, N'Simone', null, N'Weil', null, CURRENT_TIMESTAMP, 1),
(22, N'Oscar', null, N'Wilde', null, CURRENT_TIMESTAMP, 1)
;

SET IDENTITY_INSERT dbo.Authors OFF;
GO


-- Quotes.
SET IDENTITY_INSERT dbo.Quotes ON;
GO

INSERT INTO Quotes (QuoteId, AuthorId, QuoteTxt, CreatedDtm, VersionNum)
VALUES(1, 1, N'When people in a democracy are not educated in the art of living --- to strengthen their conscience, compassion, '
			+ N'and ability to question and think critically --- they can be easily manipulated by fear and propaganda. A democracy is '
			+ N'only as wise as its citizens, and a democracy of ignorant citizens can be as dangerous as a dictatorship.',
			CURRENT_TIMESTAMP, 1),
(2, 2, N'God created war so that Americans would learn geography.', CURRENT_TIMESTAMP, 1),
(3, 3, N'People shouldn''t be afraid of their government. Governments should be afraid of their people.', CURRENT_TIMESTAMP, 1),
(4, 4, N'What difference does it make to the dead, the orphans and the homeless, whether the mad destruction ' +
	+ N'is wrought under the name of totalitarianism or in the holy name of liberty or democracy?', CURRENT_TIMESTAMP, 1),
(5, 5, 'The first duty of a man is to think for himself', CURRENT_TIMESTAMP, 1),
(6, 6, N'The best argument against democracy is a five-minute conversation with the average voter.', CURRENT_TIMESTAMP, 1),
(7, 7, N'But collective thinking is usually short-lived. We''re fickle, stupid beings with poor memories and a '
	+ N'great gift for self-destruction.', CURRENT_TIMESTAMP, 1),
(8, 2, N'If voting made any difference they wouldn''t let us do it.', CURRENT_TIMESTAMP, 1),
(9, 8, N'You show me a capitalist, and I''ll show you a bloodsucker', CURRENT_TIMESTAMP, 1),
(10, 9, N'The leaders are lizards. The people hate the lizards and the lizards rule the people. "'
	+ N'"Odd," said Arthur, "I thought you said it was a democracy." "I did," said Ford. "It is."', CURRENT_TIMESTAMP, 1),
(11, 10, N'Civil government, so far as it is instituted for the security of property, is in reality instituted for the defense '
	+ N'of the rich against the poor, or of those who have some property against those who have none at all.', CURRENT_TIMESTAMP, 1),
(12, 11, N'A democracy cannot exist as a permanent form of government. It can only exist until the majority discovers it can vote itself '
	+ N'largess out of the public treasury. After that, the majority always votes for the candidate promising the most benefits with the '
	+ N'result the democracy collapses because of the loose fiscal policy ensuing, always to be followed by a dictatorship, then a monarchy.', CURRENT_TIMESTAMP, 1),
(13, 12, N'In politics as in philosophy, my tenets are few and simple. The leading one of which, and indeed that which embraces '
	+ N'most others, is to be honest and just ourselves and to exact it from others, meddling as little as possible in their affairs where '
	+ N'our own are not involved. If this maxim was generally adopted, wars would cease and our swords would soon be converted into reap hooks '
	+ N'and our harvests be more peaceful, abundant, and happy.', CURRENT_TIMESTAMP, 1),
(14, 13, N'Democracy is a pathetic belief in the collective wisdom of individual ignorance. No one in this world, so far as I know—and I '
	+ N'have researched the records for years, and employed agents to help me—has ever lost money by underestimating the intelligence of the '
	+ N'great masses of the plain people. Nor has anyone ever lost public office thereby.', CURRENT_TIMESTAMP, 1),
(15, 14, N'A vote is like a rifle: its usefulness depends upon the character of the user.', CURRENT_TIMESTAMP, 1),
(16, 15, N'Propaganda is to a democracy what the bludgeon is to a totalitarian state.', CURRENT_TIMESTAMP, 1),
(17, 16, N'The liberty of a democracy is not safe if the people tolerated the growth of private power to a point where it becomes stronger '
	+ N'than the democratic state itself. That in its essence is fascism: ownership of government by an individual, by a group, or any '
	+ N'controlling private power.', CURRENT_TIMESTAMP, 1),
(18, 17, N'Anarchism is democracy taken seriously.', CURRENT_TIMESTAMP, 1),
(19, 8, N'And when I speak, I don''t speak as a Democrat. Or a Republican. Nor an American. I speak as a victim of America''s so-called '
	+ N'democracy. You and I have never seen democracy - all we''ve seen is hypocrisy. When we open our eyes today and look around America, '
	+ N'we see America not through the eyes of someone who has enjoyed the fruits of Americanism. We see America through the eyes of someone '
	+ N'who has been the victim of Americanism. We don''t see any American dream. We''ve experienced only the American nightmare.', CURRENT_TIMESTAMP, 1),
(20, 18, N'Politics: “Poli” a Latin word meaning "many" and "tics" meaning "bloodsucking creatures".', CURRENT_TIMESTAMP, 1),
(21, 19, N'The greatest purveyor of violence in the world : My own Government, I can not be Silent.', CURRENT_TIMESTAMP, 1),
(22, 21, N'Whether the mask is labeled fascism, democracy, or dictatorship of the proletariat, our great adversary remains the apparatus—the '
	+ N'bureaucracy, the police, the military. Not the one facing us across the frontier of the battle lines, which is not so much our enemy '
	+ N'as our brothers'' enemy, but the one that calls itself our protector and makes us its slaves. No matter what the circumstances, the '
	+ N'worst betrayal will always be to subordinate ourselves to this apparatus and to trample underfoot, in its service, all human values '
	+ N'in ourselves and in others.', CURRENT_TIMESTAMP, 1),
(23, 22, N'High hopes were once formed of democracy; but democracy means simply the bludgeoning of the people by the people for the people.', CURRENT_TIMESTAMP, 1)
;

SET IDENTITY_INSERT dbo.Quotes OFF;
GO




















