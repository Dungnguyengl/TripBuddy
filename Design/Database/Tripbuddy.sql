CREATE SCHEMA [TRIPPER]
GO

CREATE SCHEMA [AUDIT]
GO

CREATE SCHEMA [MEDIA]
GO

CREATE SCHEMA [USER]
GO

CREATE SCHEMA [SPOT]
GO

CREATE SCHEMA [EVALUATE]
GO

CREATE TABLE [TRIPPER].[TRIP_HEAD] (
  [TRIP_KEY] uniqueidentifier DEFAULT (NEWID()),
  [START_DATE] datetime2,
  [END_DATE] datetime2,
  PRIMARY KEY ([TRIP_KEY])
)
GO

CREATE TABLE [TRIPPER].[TRIP_PLC] (
  [TRIP_KEY] uniqueidentifier,
  [REFERENT_KEY] uniqueidentifier,
  [PLC_TYPE] nvarchar(255) NOT NULL CHECK ([PLC_TYPE] IN ('Atraction', 'Destination', 'Place')),
  [PLC_CONTENT] nvarchar(100)
)
GO

CREATE TABLE [AUDIT].[AUDIT_HEAD] (
  [REFERENCE_KEY] uniqueidentifier,
  [SRC] nvarchar(255) NOT NULL CHECK ([SRC] IN ('USER', 'ATR', 'DES', 'PLC', 'TRIP', 'EVL', 'EVL_RATE', 'MAIL', 'NOTI')),
  [INS_BY] uniqueidentifier,
  [INS_DATE] datetime2,
  [UPD_BY] uniqueidentifier,
  [UPD_dATE] datetime2
)
GO

CREATE TABLE [AUDIT].[AUDIT_LOG] (
  [REFERENCE_KEY] uniqueidentifier,
  [SRC] nvarchar(255) NOT NULL CHECK ([SRC] IN ('USER', 'ATR', 'DES', 'PLC', 'TRIP', 'EVL', 'EVL_RATE', 'MAIL', 'NOTI')),
  [REASON] nvarchar(255) NOT NULL CHECK ([REASON] IN ('CREATE', 'UPDATE', 'DELETE')),
  [INS_BY] uniqueidentifier,
  [INS_DATE] datetime2,
  [UPD_BY] uniqueidentifier,
  [UPD_dATE] datetime2
)
GO

CREATE TABLE [MEDIA].[MD_HEAD] (
  [MD_KEY] uniqueidentifier DEFAULT (NEWID()),
  [MD_TYPE] nvarchar(255) NOT NULL CHECK ([MD_TYPE] IN ('IMAGE')),
  [MD_LINK] nvarchar(MAX),
  PRIMARY KEY ([MD_KEY])
)
GO

CREATE TABLE [MEDIA].[MD_DETAIL] (
  [MD_TYPE] nvarchar(255) NOT NULL CHECK ([MD_TYPE] IN ('IMAGE')),
  [MD_EXTENTION] nvarchar(50)
)
GO

CREATE TABLE [USER].[USER_HEAD] (
  [USER_KEY] uniqueidentifier DEFAULT (NEWID()),
  [FIRST_NAME] nvarchar(20),
  [LAST_NAME] nvarchar(20),
  [EMAIL] nvarchar(255),
  [PHONE_NUMBER] nvarchar(20),
  [LOCATION_KEY] uniqueidentifier,
  [LOCATION_NAME] nvarchar(255),
  [DOB] datetime2,
  [PIC_KEY] uniqueidentifier,
  [IS_ACTIVIE] bit DEFAULT (0),
  PRIMARY KEY ([USER_KEY])
)
GO

CREATE TABLE [USER].[USER_SPOT] (
  [USER_KEY] uniqueidentifier UNIQUE NOT NULL,
  [REWARD_POINT] bigint DEFAULT (0),
  [NO_TRIP] bigint DEFAULT (0),
  [NO_PLACE] bigint DEFAULT (0),
  [RATING] smallint DEFAULT (0)
)
GO

CREATE TABLE [SPOT].[ATR_HEAD] (
  [ATR_KEY] uniqueidentifier DEFAULT (NEWID()),
  [DESCRIPTION] nvarchar(MAX),
  [CONTITNENT] nvarchar(5),
  [SUB_CONTITNENT] nvarchar(5),
  [COUNTRY] nvarchar(50),
  [PIC_KEY] uniqueidentifier,
  PRIMARY KEY ([ATR_KEY])
)
GO

CREATE TABLE [SPOT].[DES_HEAD] (
  [DES_KEY] uniqueidentifier DEFAULT (NEWID()),
  [ATR_KEY] uniqueidentifier NOT NULL,
  [DES_NAME] nvarchar(50),
  [DESCRIPTION] nvarchar(MAX),
  [PIC_KEY] uniqueidentifier,
  [NO_VISTED] int DEFAULT (0),
  PRIMARY KEY ([DES_KEY])
)
GO

CREATE TABLE [SPOT].[PLC_HEAD] (
  [PLC_KEY] uniqueidentifier DEFAULT (NEWID()),
  [ATR_KEY] uniqueidentifier NOT NULL,
  [DES_KEY] uniqueidentifier NOT NULL,
  [PLC_NAME] nvarchar(50),
  [DESCRIPTION] nvarchar(MAX),
  [LONGITUDE] float,
  [LATITUDE] float,
  [PIC_KEY] uniqueidentifier,
  PRIMARY KEY ([PLC_KEY])
)
GO

CREATE TABLE [SPOT].[ATR_CONTENT] (
  [CONTENT_KEY] uniqueidentifier DEFAULT (NEWID()),
  [ATR_KEY] uniqueidentifier NOT NULL,
  [DES_KEY] uniqueidentifier NOT NULL,
  [CONTENT_TYPE] nvarchar(5),
  [TITLE] nvarchar(100),
  [CONTENT] nvarchar(MAX),
  PRIMARY KEY ([CONTENT_KEY])
)
GO

CREATE TABLE [SPOT].[CONSTANT] (
  [CONSTANT_CODE] nvarchar(5),
  [CONSTANT_TYPE] nvarchar(30),
  [CONSTANT_NAME] nvarchar(30)
)
GO

CREATE TABLE [EVALUATE].[EVL_HEAD] (
  [EVL_KEY] uniqueidentifier PRIMARY KEY DEFAULT (NEWID()),
  [EVL_TITLE] nvarchar(100),
  [EVL_CONTENT] nvarchar(MAX),
  [PIC_KEY] uniqueidentifier
)
GO

CREATE TABLE [EVALUATE].[EVL_RATE] (
  [EVL_KEY] uniqueidentifier,
  [RATE] smallint
)
GO

CREATE TABLE [EVALUATE].[EVL_PLC] (
  [EVL_KEY] uniqueidentifier,
  [ATR_KEY] uniqueidentifier,
  [DES_KEY] uniqueidentifier,
  [PLC_KEY] uniqueidentifier,
  [PLC_CONENT] nvarchar(100)
)
GO

CREATE INDEX [TRIP_HEAD_index_0] ON [TRIPPER].[TRIP_HEAD] ("START_DATE")
GO

CREATE INDEX [TRIP_HEAD_index_1] ON [TRIPPER].[TRIP_HEAD] ("START_DATE", "END_DATE")
GO

CREATE INDEX [AUDIT_HEAD_index_0] ON [AUDIT].[AUDIT_HEAD] ("INS_BY")
GO

CREATE INDEX [AUDIT_HEAD_index_1] ON [AUDIT].[AUDIT_HEAD] ("INS_BY", "INS_DATE")
GO

CREATE INDEX [AUDIT_HEAD_index_2] ON [AUDIT].[AUDIT_HEAD] ("UPD_BY")
GO

CREATE INDEX [AUDIT_HEAD_index_3] ON [AUDIT].[AUDIT_HEAD] ("UPD_BY", "UPD_dATE")
GO

CREATE INDEX [AUDIT_LOG_index_4] ON [AUDIT].[AUDIT_LOG] ("REFERENCE_KEY", "REASON")
GO

CREATE INDEX [AUDIT_LOG_index_5] ON [AUDIT].[AUDIT_LOG] ("INS_BY")
GO

CREATE INDEX [AUDIT_LOG_index_6] ON [AUDIT].[AUDIT_LOG] ("INS_BY", "INS_DATE")
GO

CREATE INDEX [AUDIT_LOG_index_7] ON [AUDIT].[AUDIT_LOG] ("UPD_BY")
GO

CREATE INDEX [AUDIT_LOG_index_8] ON [AUDIT].[AUDIT_LOG] ("UPD_BY", "UPD_dATE")
GO

CREATE INDEX [MD_HEAD_index_0] ON [MEDIA].[MD_HEAD] ("MD_TYPE")
GO

CREATE INDEX [USER_HEAD_index_0] ON [USER].[USER_HEAD] ("FIRST_NAME", "LAST_NAME")
GO

CREATE UNIQUE INDEX [USER_SPOT_index_1] ON [USER].[USER_SPOT] ("USER_KEY")
GO

CREATE INDEX [ATR_HEAD_index_0] ON [SPOT].[ATR_HEAD] ("CONTITNENT")
GO

CREATE INDEX [ATR_HEAD_index_1] ON [SPOT].[ATR_HEAD] ("CONTITNENT", "SUB_CONTITNENT")
GO

CREATE INDEX [DES_HEAD_index_2] ON [SPOT].[DES_HEAD] ("ATR_KEY")
GO

CREATE INDEX [PLC_HEAD_index_3] ON [SPOT].[PLC_HEAD] ("ATR_KEY", "DES_KEY")
GO

CREATE INDEX [ATR_CONTENT_index_4] ON [SPOT].[ATR_CONTENT] ("ATR_KEY", "DES_KEY")
GO

CREATE INDEX [EVL_RATE_index_0] ON [EVALUATE].[EVL_RATE] ("EVL_KEY")
GO

CREATE INDEX [EVL_PLC_index_1] ON [EVALUATE].[EVL_PLC] ("EVL_KEY")
GO

CREATE INDEX [EVL_PLC_index_2] ON [EVALUATE].[EVL_PLC] ("ATR_KEY", "DES_KEY", "PLC_KEY")
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Number of trip',
@level0type = N'Schema', @level0name = 'USER',
@level1type = N'Table',  @level1name = 'USER_SPOT',
@level2type = N'Column', @level2name = 'NO_TRIP';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Number of place',
@level0type = N'Schema', @level0name = 'USER',
@level1type = N'Table',  @level1name = 'USER_SPOT',
@level2type = N'Column', @level2name = 'NO_PLACE';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Average rating from user''s review',
@level0type = N'Schema', @level0name = 'USER',
@level1type = N'Table',  @level1name = 'USER_SPOT',
@level2type = N'Column', @level2name = 'RATING';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Number of user add this description into their schedule',
@level0type = N'Schema', @level0name = 'SPOT',
@level1type = N'Table',  @level1name = 'DES_HEAD',
@level2type = N'Column', @level2name = 'NO_VISTED';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'Specific location',
@level0type = N'Schema', @level0name = 'EVALUATE',
@level1type = N'Table',  @level1name = 'EVL_PLC',
@level2type = N'Column', @level2name = 'PLC_CONENT';
GO

ALTER TABLE [USER].[USER_HEAD] ADD FOREIGN KEY ([USER_KEY]) REFERENCES [USER].[USER_SPOT] ([USER_KEY])
GO

ALTER TABLE [SPOT].[DES_HEAD] ADD FOREIGN KEY ([ATR_KEY]) REFERENCES [SPOT].[ATR_HEAD] ([ATR_KEY])
GO

ALTER TABLE [SPOT].[PLC_HEAD] ADD FOREIGN KEY ([ATR_KEY]) REFERENCES [SPOT].[ATR_HEAD] ([ATR_KEY])
GO

ALTER TABLE [SPOT].[PLC_HEAD] ADD FOREIGN KEY ([DES_KEY]) REFERENCES [SPOT].[DES_HEAD] ([DES_KEY])
GO

ALTER TABLE [SPOT].[ATR_CONTENT] ADD FOREIGN KEY ([ATR_KEY]) REFERENCES [SPOT].[ATR_HEAD] ([ATR_KEY])
GO

ALTER TABLE [SPOT].[ATR_CONTENT] ADD FOREIGN KEY ([DES_KEY]) REFERENCES [SPOT].[DES_HEAD] ([DES_KEY])
GO

ALTER TABLE [EVALUATE].[EVL_PLC] ADD FOREIGN KEY ([EVL_KEY]) REFERENCES [EVALUATE].[EVL_HEAD] ([EVL_KEY])
GO

ALTER TABLE [EVALUATE].[EVL_RATE] ADD FOREIGN KEY ([EVL_KEY]) REFERENCES [EVALUATE].[EVL_HEAD] ([EVL_KEY])
GO

ALTER TABLE [TRIPPER].[TRIP_PLC] ADD FOREIGN KEY ([TRIP_KEY]) REFERENCES [TRIPPER].[TRIP_HEAD] ([TRIP_KEY])
GO
