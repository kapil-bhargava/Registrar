IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SeatMatrix')
BEGIN
    CREATE TABLE SeatMatrix
    (
        SeatMatrixId       INT IDENTITY(1,1) PRIMARY KEY,
        CourseId           INT NOT NULL,
        AcademicSessionId  INT NOT NULL,
        TotalSeats         INT NOT NULL,
        IsActive           BIT NOT NULL DEFAULT 1,
        CreatedDate        DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_SeatMatrix_Course FOREIGN KEY (CourseId) REFERENCES CourseMaster(CourseId),
        CONSTRAINT FK_SeatMatrix_Session FOREIGN KEY (AcademicSessionId) REFERENCES AcademicSession(AcademicSessionId)
    );
END
GO