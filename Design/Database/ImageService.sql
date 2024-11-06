CREATE SCHEMA [IMAGE]
GO

CREATE TABLE [IMAGE].[IMAGE_HEAD] (
  [IMAGE_KEY] uniqueidentifier PRIMARY KEY,
  [IS_DELETE] bit,
  [DELETE_DATE] datetime2,
  [IMAGE_TYPE] nvarchar(20)
)
GO
