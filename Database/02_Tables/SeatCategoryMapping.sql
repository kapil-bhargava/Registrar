IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SeatCategoryMapping')
BEGIN
    CREATE TABLE SeatCategoryMapping
    (
        SeatCategoryMappingId INT IDENTITY(1,1) PRIMARY KEY,
        SeatMatrixId          INT NOT NULL,
        CategoryId            INT NOT NULL,
        AllocatedSeats        INT NOT NULL,
        CreatedDate           DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_SCM_SeatMatrix FOREIGN KEY (SeatMatrixId) REFERENCES SeatMatrix(SeatMatrixId) ON DELETE CASCADE,
        CONSTRAINT FK_SCM_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
    );
END
GO