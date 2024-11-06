-- Set the database context to db_aaedbd_tripbuddy
USE db_aaedbd_tripbuddy;
GO

-- Check if the PIC_KEY column exists and change its data type to uniqueidentifier if it does
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_SCHEMA = 'TRIPPER' 
             AND TABLE_NAME = 'TRIP_HEAD' 
             AND COLUMN_NAME = 'PIC_KEY')
BEGIN
    ALTER TABLE TRIPPER.TRIP_HEAD
    ALTER COLUMN PIC_KEY UNIQUEIDENTIFIER NOT NULL;
END
ELSE
BEGIN
    -- If PIC_KEY column does not exist, add it with uniqueidentifier type
    ALTER TABLE TRIPPER.TRIP_HEAD
    ADD PIC_KEY UNIQUEIDENTIFIER NOT NULL;
END

-- Check and add IS_VISITED column if it does not exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_SCHEMA = 'TRIPPER' 
                 AND TABLE_NAME = 'TRIP_HEAD' 
                 AND COLUMN_NAME = 'IS_VISITED')
BEGIN
    ALTER TABLE TRIPPER.TRIP_HEAD
    ADD IS_VISITED BIT NULL;
END

-- Check and add IS_DELETED column if it does not exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_SCHEMA = 'TRIPPER' 
                 AND TABLE_NAME = 'TRIP_HEAD' 
                 AND COLUMN_NAME = 'IS_DELETED')
BEGIN
    ALTER TABLE TRIPPER.TRIP_HEAD
    ADD IS_DELETED BIT NULL;
END
