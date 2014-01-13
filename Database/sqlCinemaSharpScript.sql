-- ----------------------------------------------------------------------
-- Name: Sergey Furtakov
-- Abstract: Database for Cinema project
-- Sergey.Furtakov@gmail.com
-- ----------------------------------------------------------------------

-- ----------------------------------------------------------------------
-- Options
-- ----------------------------------------------------------------------
USE dbCinema
SET NOCOUNT ON	-- Report only errors


IF OBJECT_ID( 'TPlaceDateShowTimeRowSeats' )	IS NOT NULL DROP TABLE TPlaceDateShowTimeRowSeats
IF OBJECT_ID( 'TSeats' )						IS NOT NULL DROP TABLE TSeats
IF OBJECT_ID( 'TPrices' )						IS NOT NULL DROP TABLE TPrices
IF OBJECT_ID( 'TSellStatuses' )					IS NOT NULL DROP TABLE TSellStatuses
IF OBJECT_ID( 'TPlaceDateShowTimeRows' )		IS NOT NULL DROP TABLE TPlaceDateShowTimeRows
IF OBJECT_ID( 'TRows' )							IS NOT NULL DROP TABLE TRows
IF OBJECT_ID( 'TPlaceDateShowTimes' )			IS NOT NULL DROP TABLE TPlaceDateShowTimes
IF OBJECT_ID( 'TShowTimes' )					IS NOT NULL DROP TABLE TShowTimes
IF OBJECT_ID( 'TShowNames' )					IS NOT NULL DROP TABLE TShowNames
IF OBJECT_ID( 'TPlaceDates' )					IS NOT NULL DROP TABLE TPlaceDates
IF OBJECT_ID( 'TPlaces' )						IS NOT NULL DROP TABLE TPlaces

IF OBJECT_ID( 'uspAddDate' )					IS NOT NULL DROP PROCEDURE uspAddDate
IF OBJECT_ID( 'uspAddShowName' )				IS NOT NULL DROP PROCEDURE uspAddShowName
IF OBJECT_ID( 'uspAddMovie' )					IS NOT NULL DROP PROCEDURE uspAddMovie

IF OBJECT_ID( 'uspGetListOfMovies' )			IS NOT NULL DROP PROCEDURE uspGetListOfMovies
IF OBJECT_ID( 'uspGetSeatSchema' )				IS NOT NULL DROP PROCEDURE uspGetSeatSchema

IF OBJECT_ID( 'uspPickSeats' )					IS NOT NULL DROP PROCEDURE uspPickSeats
IF OBJECT_ID( 'uspPickSeats2' )					IS NOT NULL DROP PROCEDURE uspPickSeats2



CREATE TABLE TPlaces
(
	intPlaceID                 Integer        NOT NULL,
	strPlace                   VarChar(50)    NOT NULL,
	CONSTRAINT TPlaces_PK PRIMARY KEY CLUSTERED ( intPlaceID )
)
GO

CREATE TABLE TPlaceDates
(
	intPlaceID                 Integer        NOT NULL,
	intDateIndex               Integer        NOT NULL,
	dtmDate                    DateTime       NOT NULL,
	CONSTRAINT TPlaceDates_PK PRIMARY KEY CLUSTERED ( intPlaceID, intDateIndex )
)
GO

CREATE TABLE TPlaceDateShowTimes
(
	intPlaceID                 Integer        NOT NULL,
	intDateIndex               Integer        NOT NULL,
	intShowTimeID              Integer        NOT NULL,
	intShowNameID              Integer        NOT NULL,
	CONSTRAINT TPlaceDateShowTimes_PK PRIMARY KEY CLUSTERED ( intPlaceID, intDateIndex, intShowTimeID )
)
GO

CREATE TABLE TShowTimes
(
	intShowTimeID              Integer        NOT NULL,
	strShowTime                VarChar(50)    NOT NULL,
	CONSTRAINT TShowTimes_PK PRIMARY KEY CLUSTERED ( intShowTimeID )
)
GO

CREATE TABLE TShowNames
(
	intShowNameID              Integer        NOT NULL,
	strShowName                VarChar(50)    NOT NULL,
	CONSTRAINT TShowNames_PK PRIMARY KEY CLUSTERED ( intShowNameID )
)
GO

CREATE TABLE TPlaceDateShowTimeRows
(
	intPlaceID                 Integer        NOT NULL,
	intDateIndex               Integer        NOT NULL,
	intShowTimeID              Integer        NOT NULL,
	intRowID                   Integer        NOT NULL,
	CONSTRAINT TPlaceDateShowTimeRows_PK PRIMARY KEY CLUSTERED ( intPlaceID, intDateIndex, intShowTimeID, intRowID )
)
GO

CREATE TABLE TRows
(
	intRowID                   Integer        NOT NULL,
	strRow                     VarChar(50)    NOT NULL,
	CONSTRAINT TRows_PK PRIMARY KEY CLUSTERED ( intRowID )
)
GO

CREATE TABLE TPlaceDateShowTimeRowSeats
(
	intPlaceID                 Integer        NOT NULL,
	intDateIndex               Integer        NOT NULL,
	intShowTimeID              Integer        NOT NULL,
	intRowID                   Integer        NOT NULL,
	intSeatID                  Integer        NOT NULL,
	intPriceID                 Integer        NOT NULL,
	intSellStatusID            Integer        NOT NULL,
	CONSTRAINT TPlaceDateShowTimeRowSeats_PK PRIMARY KEY CLUSTERED ( intPlaceID, intDateIndex, intShowTimeID, intRowID, intSeatID )
)
GO

CREATE TABLE TSeats
(
	intSeatID                  Integer        NOT NULL,
	strSeat                    VarChar(50)    NOT NULL,
	CONSTRAINT TSeats_PK PRIMARY KEY CLUSTERED ( intSeatID )
)
GO

CREATE TABLE TPrices
(
	intPriceID                 Integer        NOT NULL,
	curPrice                   Money          NOT NULL,
	CONSTRAINT TPrices_PK PRIMARY KEY CLUSTERED ( intPriceID )
)
GO

CREATE TABLE TSellStatuses
(
	intSellStatusID            Integer        NOT NULL,
	strSellStatus              VarChar(50)    NOT NULL,
	CONSTRAINT TSellStatuses_PK PRIMARY KEY CLUSTERED ( intSellStatusID )
)
GO


----------------------------------------------------------------------
-- Foreign Keys
-- ----------------------------------------------------------------------
ALTER TABLE TPlaceDates ADD CONSTRAINT TPlaceDates_TPlaces_FK1
FOREIGN KEY( intPlaceID ) REFERENCES TPlaces ( intPlaceID )
GO

ALTER TABLE TPlaceDateShowTimes ADD CONSTRAINT TPlaceDateShowTimes_TPlaceDates_FK1
FOREIGN KEY( intPlaceID, intDateIndex ) REFERENCES TPlaceDates ( intPlaceID, intDateIndex )
GO

ALTER TABLE TPlaceDateShowTimes ADD CONSTRAINT TPlaceDateShowTimes_TShowNames_FK1
FOREIGN KEY( intShowNameID ) REFERENCES TShowNames ( intShowNameID )
GO

ALTER TABLE TPlaceDateShowTimeRowSeats ADD CONSTRAINT TPlaceDateShowTimeRowSeats_TPrices_FK1
FOREIGN KEY( intPriceID ) REFERENCES TPrices ( intPriceID )
GO

ALTER TABLE TPlaceDateShowTimeRowSeats ADD CONSTRAINT TPlaceDateShowTimeRowSeats_TSellStatuses_FK1
FOREIGN KEY( intSellStatusID ) REFERENCES TSellStatuses ( intSellStatusID )
GO

ALTER TABLE TPlaceDateShowTimes ADD CONSTRAINT TPlaceDateShowTimes_TShowTimes_FK1
FOREIGN KEY( intShowTimeID ) REFERENCES TShowTimes ( intShowTimeID )
GO

ALTER TABLE TPlaceDateShowTimeRows ADD CONSTRAINT TPlaceDateShowTimeRows_TPlaceDateShowTimes_FK1
FOREIGN KEY( intPlaceID, intDateIndex, intShowTimeID ) REFERENCES TPlaceDateShowTimes ( intPlaceID, intDateIndex, intShowTimeID )
GO

ALTER TABLE TPlaceDateShowTimeRows ADD CONSTRAINT TPlaceDateShowTimeRows_TRows_FK1
FOREIGN KEY( intRowID ) REFERENCES TRows ( intRowID )
GO

ALTER TABLE TPlaceDateShowTimeRowSeats ADD CONSTRAINT TPlaceDateShowTimeRowSeats_TPlaceDateShowTimeRows_FK1
FOREIGN KEY( intPlaceID, intDateIndex, intShowTimeID, intRowID ) REFERENCES TPlaceDateShowTimeRows ( intPlaceID, intDateIndex, intShowTimeID, intRowID )
GO

ALTER TABLE TPlaceDateShowTimeRowSeats ADD CONSTRAINT TPlaceDateShowTimeRowSeats_TSeats_FK1
FOREIGN KEY( intSeatID ) REFERENCES TSeats ( intSeatID )
GO


-- ----------------------------------------------------------------------
-- Inserts
-- ----------------------------------------------------------------------
INSERT INTO TPlaces( intPlaceID, strPlace )
VALUES( 1, 'Parent Cinema' )

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 1, '10:00 AM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 2, '12:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 3, '2:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 4, '4:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 5, '6:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 6, '8:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 7, '10:00 PM' )	

INSERT INTO TShowTimes( intShowTimeID, strShowTime )
VALUES( 8, '12:00 PM' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 1, '01' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 2, '02' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 3, '03' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 4, '04' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 5, '05' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 6, '06' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 7, '07' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 8, '08' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 9, '09' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 10, '10' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 11, '11' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 12, '12' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 13, '13' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 14, '14' )	

INSERT INTO TRows( intRowID, strRow )
VALUES( 15, '15' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 1, '01' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 2, '02' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 3, '03' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 4, '04' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 5, '05' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 6, '06' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 7, '07' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 8, '08' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 9, '09' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 10, '10' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 11, '11' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 12, '12' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 13, '13' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 14, '14' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 15, '15' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 16, '16' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 17, '17' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 18, '18' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 19, '19' )	

INSERT INTO TSeats( intSeatID, strSeat )
VALUES( 20, '20' )	


INSERT INTO TPrices( intPriceID, curPrice )
VALUES( 1, 5 )

INSERT INTO TPrices( intPriceID, curPrice )
VALUES( 2, 7 )

INSERT INTO TPrices( intPriceID, curPrice )
VALUES( 3, 10 )

INSERT INTO TPrices( intPriceID, curPrice )
VALUES( 4, 13 )

INSERT INTO TSellStatuses( intSellStatusID, strSellStatus )
VALUES( 1, 'Available' )

INSERT INTO TSellStatuses( intSellStatusID, strSellStatus )
VALUES( 2, 'Sold' )
GO


-- ----------------------------------------------------------------------
-- Procedures
-- ----------------------------------------------------------------------

-- ----------------------------------------------------------------------
-- Add Date
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspAddDate
	 @intPlaceID			AS INTEGER
	,@dtmDate				AS DATE
	,@intDateIndex			AS INTEGER OUTPUT
AS
SET NOCOUNT ON		-- Report only errors
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

-- Check if date is already exists
DECLARE @blnAlreadyExists AS BIT = 0 -- False, does not exist

BEGIN TRANSACTION

	SELECT
		 @blnAlreadyExists	= 1
		,@intDateIndex		= intDateIndex
	FROM
		TPlaceDates (TABLOCKX) -- Lock table until end of transaction
	WHERE	intPlaceID			=	@intPlaceID
		AND	dtmDate				=	@dtmDate
				
	IF @blnAlreadyExists = 0	-- Date does NOT exists, so add it
	BEGIN

		SELECT @intDateIndex = MAX( intDateIndex ) + 1
		FROM TPlaceDates (TABLOCKX) -- Lock table until end of transaction
		
		-- Default to 1 if table is empty
		SELECT @intDateIndex = COALESCE( @intDateIndex, 1 )
		
		INSERT INTO TPlaceDates( intPlaceID, intDateIndex, dtmDate )
		VALUES( @intPlaceID, @intDateIndex, @dtmDate )
		
		-- Return value to caller
		SELECT @intDateIndex AS intDateIndex
	
	END
		
COMMIT TRANSACTION

GO


-- ----------------------------------------------------------------------
-- Add Show name
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspAddShowName
	 @strShowName			AS VARCHAR(50)
	,@intShowNameID			AS INTEGER OUTPUT
AS
SET NOCOUNT ON		-- Report only errors
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

-- Check if Show name already exists
DECLARE @blnAlreadyExists AS BIT = 0 -- False, does not exist

BEGIN TRANSACTION
	
	-- Check if movie exists
	SELECT
		 @blnAlreadyExists = 1
		,@intShowNameID = intShowNameID
	FROM
		TShowNames (TABLOCKX) -- Lock table until end of transaction
	WHERE	strShowName			=	@strShowName
				
	IF @blnAlreadyExists = 0	-- Show name does NOT exists, so add it
	BEGIN
	
		SELECT @intShowNameID = MAX( intShowNameID ) + 1
		FROM TShowNames (TABLOCKX) -- Lock table until end of transaction
		
		-- Default to 1 if table is empty
		SELECT @intShowNameID = COALESCE( @intShowNameID, 1 )
		
		INSERT INTO TShowNames( intShowNameID, strShowName )
		VALUES( @intShowNameID, @strShowName )
		
		-- Return value to caller
		SELECT @intShowNameID AS intShowNameID
	END
	
COMMIT TRANSACTION

GO


-- ----------------------------------------------------------------------
-- Add movie
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspAddMovie
	 @intPlaceID			AS INTEGER
	,@dtmDate				AS DATE
	,@intShowTimeID			AS INTEGER
	,@strShowName			AS VARCHAR( 50 )
	,@intPriceID			AS INTEGER
AS
SET NOCOUNT ON		-- Report only errors
SET XACT_ABORT ON	-- Terminate and rollback entire transaction on error

BEGIN TRANSACTION

DECLARE @intDateIndex	AS INTEGER;
DECLARE @intShowNameID	AS INTEGER;

---- Add date to TPlaceDates
EXECUTE uspAddDate @intPlaceID, @dtmDate, @intDateIndex OUTPUT

-- Add Show name to TShowNames
EXECUTE uspAddShowName @strShowName, @intShowNameID	OUTPUT

-- Update TPlaceDateShowTimes
INSERT INTO TPlaceDateShowTimes( intPlaceID, intDateIndex, intShowTimeID, intShowNameID )
VALUES( @intPlaceID, @intDateIndex, @intShowTimeID, @intShowNameID )

-- Update TPlaceDateShowTimeRows
DECLARE @intRowIndex		AS INTEGER = 1;
DECLARE @intRowsTotal	AS INTEGER = 0;

-- Get the total number of rows
SELECT @intRowsTotal = COUNT( * ) FROM TRows

-- Update
START1:	-- DO

	INSERT INTO TPlaceDateShowTimeRows( intPlaceID, intDateIndex, intShowTimeID, intRowID )
	VALUES( @intPlaceID, @intDateIndex, @intShowTimeID, @intRowIndex )
	SET @intRowIndex +=1;
	
IF @intRowIndex <= @intRowsTotal GOTO START1; -- WHILE 

-- Update TPlaceDateShowTimeRowSeats
DECLARE @intSeatIndex		AS INTEGER = 1;
DECLARE @intSeatsTotal	AS INTEGER = 0;

-- Get the total number of rows
SELECT @intSeatsTotal = COUNT( * ) FROM TSeats

SET @intRowIndex = 1;

-- Go through every row
START2:	-- DO

	SET @intSeatIndex = 1;
	
	-- Add seats
	START3:	-- DO

		INSERT INTO TPlaceDateShowTimeRowSeats( intPlaceID, intDateIndex, intShowTimeID, intRowID, intSeatID, intPriceID, intSellStatusID )
		VALUES( @intPlaceID, @intDateIndex, @intShowTimeID, @intRowIndex, @intSeatIndex, @intPriceID, 1 )
		SET @intSeatIndex +=1;
		
	IF @intSeatIndex <= @intSeatsTotal GOTO START3; -- WHILE 
	SET @intRowIndex +=1;
	
IF @intRowIndex <= @intRowsTotal GOTO START2; -- WHILE 
	
COMMIT TRANSACTION

GO


-- ----------------------------------------------------------------------
-- Inserts (Movies)
-- ----------------------------------------------------------------------

GO
uspAddMovie 1, '20131213', 1, 'Zorro', 1		
GO
uspAddMovie 1, '20131213', 2, 'Zorro', 1				
GO
uspAddMovie 1, '20131213', 3, 'Coffee and Cigarettes', 3
GO
uspAddMovie 1, '20131213', 4, 'Zorro', 4
GO
uspAddMovie 1, '20131214', 1, 'Hobbit 2', 1		
GO
uspAddMovie 1, '20131214', 2, 'Hobbit 2', 1				
GO
uspAddMovie 1, '20131214', 4, 'Hobbit 2', 3
GO
uspAddMovie 1, '20131214', 5, 'Terminator 2', 4
GO


-- ----------------------------------------------------------------------
-- Get List of Movies
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspGetListOfMovies
	 @intPlaceID			AS INTEGER
	,@intDateIndex			AS INTEGER
AS
SET NOCOUNT ON		-- Report only errors

SELECT
	 TP.intPlaceID
	,TP.strPlace
	,TPD.intDateIndex
	,TPD.dtmDate
	,TST.intShowTimeID
	,TST.strShowTime
	,TSN.intShowNameID
	,TSN.strShowName
FROM
	 TPlaces				AS TP
	,TPlaceDates			AS TPD
	,TShowNames				AS TSN
	,TPlaceDateShowTimes	AS TPDST
	,TShowTimes				AS TST
WHERE
	TP.intPlaceID		= TPD.intPlaceID
AND TPD.intDateIndex	= TPDST.intDateIndex
AND TPDST.intPlaceID	= TPD.intPlaceID		
AND TPDST.intDateIndex	= TPD.intDateIndex		
AND TPDST.intShowNameID	= TSN.intShowNameID
AND TPDST.intShowTimeID	= TST.intShowTimeID
AND	TP.intPlaceID		= @intPlaceID
AND TPD.intDateIndex	= @intDateIndex
ORDER BY strShowTime
GO


-- ----------------------------------------------------------------------
-- Get Seat Schema
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspGetSeatSchema
	 @intPlaceID			AS INTEGER
	,@intDateIndex			AS INTEGER
	,@intShowTimeID			AS INTEGER
AS
SET NOCOUNT ON		-- Report only errors

SELECT 
	 TP.intPlaceID
	,TP.strPlace
	,TPD.intDateIndex
	,TPD.dtmDate
	,TST.intShowTimeID
	,TST.strShowTime
	,TSN.intShowNameID
	,TSN.strShowName
	,TR.intRowID
	,TR.strRow	
	,TS.intSeatID
	,TS.strSeat
	,TPr.intPriceID
	,TPr.curPrice
	,TSS.intSellStatusID
	,TSS.strSellStatus
	
FROM
	 TPlaces					AS TP
		JOIN TPlaceDates				AS TPD	-- TP-TPD

			JOIN TPlaceDateShowTimes		AS TPDST	-- TPD-TPDST
			
				JOIN TPlaceDateShowTimeRows		AS TPDSTR	-- TPDST-TPDSTR
				
					JOIN TPlaceDateShowTimeRowSeats	AS TPDSTRS	-- TPDSTR-TPDSTRS
					ON ( TPDSTR.intPlaceID		= TPDSTRS.intPlaceID
					AND  TPDSTR.intDateIndex	= TPDSTRS.intDateIndex
					AND  TPDSTR.intShowTimeID	= TPDSTRS.intShowTimeID
					AND  TPDSTR.intRowID		= TPDSTRS.intRowID )
					
						JOIN TSeats						AS TS	-- TS-TPDSTRS
						ON ( TS.intSeatID = TPDSTRS.intSeatID)
						
						JOIN TPrices					AS TPr	-- TPr-TPDSTRS
						ON ( TPr.intPriceID = TPDSTRS.intPriceID )
						
						JOIN TSellStatuses				AS TSS	--TSS-TPDSTRS
						ON ( TSS.intSellStatusID = TPDSTRS.intSellStatusID )
						
				ON ( TPDST.intPlaceID		= TPDSTR.intPlaceID
				AND  TPDST.intDateIndex		= TPDSTR.intDateIndex
				AND  TPDST.intShowTimeID	= TPDSTR.intShowTimeID )
				
					JOIN TRows						AS TR	-- TR-TPDSTR
					ON ( TR.intRowID = TPDSTR.intRowID )

			ON ( TPD.intPlaceID		= TPDST.intPlaceID 
			AND  TPD.intDateIndex	= TPDST.intDateIndex )
				
				JOIN TShowNames					AS TSN	-- TSN-TPDST
				ON ( TSN.intShowNameID = TPDST.intShowNameID )
				
				JOIN TShowTimes					AS TST	-- TST-TPDST
				ON ( TST.intShowTimeID = TPDST.intShowTimeID )
				
		ON ( TP.intPlaceID = TPD.intPlaceID )
		
WHERE
	TPDSTRS.intPlaceID		= @intPlaceID
AND TPDSTRS.intDateIndex	= @intDateIndex
AND TPDSTRS.intShowTimeID	= @intShowTimeID

ORDER BY
	 strRow
	,strSeat
GO


-- ----------------------------------------------------------------------
-- Pick seats ( NO race condition version )
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspPickSeats
	 @intPlaceID			AS INTEGER
	,@intDateIndex			AS INTEGER
	,@intShowTimeID			AS INTEGER
	,@intRowID				AS INTEGER
	,@intSeatID				AS INTEGER
AS
SET NOCOUNT ON		-- Report only errors

UPDATE
	TPlaceDateShowTimeRowSeats

SET
	intSellStatusID = 2

WHERE
	intPlaceID		= @intPlaceID
AND	intDateIndex	= @intDateIndex
AND intShowTimeID	= @intShowTimeID
AND intRowID		= @intRowID
AND intSeatID		= @intSeatID

GO

-- ----------------------------------------------------------------------
-- Pick seats ( Race condition version )
-- ----------------------------------------------------------------------
GO

CREATE PROCEDURE uspPickSeatsWithRaceCondition
	 @intPlaceID			AS INTEGER
	,@intDateIndex			AS INTEGER
	,@intShowTimeID			AS INTEGER
	,@XMLRowSeats			AS XML
AS
SET NOCOUNT ON		-- Report only errors

BEGIN TRANSACTION

DECLARE @blnResult			BIT
DECLARE @intSeatsOrdered	INT
DECLARE @intSeatsAvailable	INT 
DECLARE	@TRowSeats			TABLE
		(
			 intRowID  INT
			,intSeatID INT
		)

BEGIN

-- Populate TRowSeats from XML 
INSERT INTO @TRowSeats (intRowID, intSeatID)
        (
			SELECT 
				 Row.id.value('@intRowID','int')
				,Row.id.value('@intSeatID','int')
			FROM 
				@XMLRowSeats.nodes('/TRowSeats/RowSeat') AS Row(id)  
		)
		
-- How many seats were ordered?
SELECT 
	@intSeatsOrdered = COUNT( * )	-- @intSeatsOrdered = COUNT( intSeatID )
FROM
	@TRowSeats
	
-- How many seats available?
SELECT
	@intSeatsAvailable = COUNT( * )	-- @intSeatsAvailable = COUNT( intSeatID )
FROM
	TPlaceDateShowTimeRowSeats (TABLOCKX) -- Lock the table so avoid multiple race condition
WHERE
	intPlaceID		= @intPlaceID
AND intDateIndex	= @intDateIndex
AND intShowTimeID	= @intShowTimeID
AND TPlaceDateShowTimeRowSeats.intRowID	IN 
	(
		SELECT 
			intRowID 
		FROM 
			@TRowSeats 
		WHERE 
			intSeatID = TPlaceDateShowTimeRowSeats.intSeatID		
	)
AND intSellStatusID = 1	-- Available

-- Are ordered seats available?
IF @intSeatsOrdered = @intSeatsAvailable

	-- Yes, so update
	BEGIN
	UPDATE
		TPlaceDateShowTimeRowSeats

	SET
		intSellStatusID = 2

	WHERE
		intPlaceID		= @intPlaceID
	AND	intDateIndex	= @intDateIndex
	AND intShowTimeID	= @intShowTimeID
	AND TPlaceDateShowTimeRowSeats.intRowID	IN 
		(
			SELECT 
				intRowID 
			FROM 
				@TRowSeats 
			WHERE 
				intSeatID = TPlaceDateShowTimeRowSeats.intSeatID		
		)

	-- Set the flag
	SELECT @blnResult = 1

	END
ELSE

	-- No, so return 0 ( Race condition occured )
	SELECT @blnResult = 0

END

COMMIT TRANSACTION

-- Return result
SELECT @blnResult

GO


-- Race condition test
--uspPickSeatsWithRaceCondition 1, 2, 1, '<TRowSeats>
--  <RowSeat intRowID="14" intSeatID="19" />
--  <RowSeat intRowID="15" intSeatID="20" />
--</TRowSeats>'
