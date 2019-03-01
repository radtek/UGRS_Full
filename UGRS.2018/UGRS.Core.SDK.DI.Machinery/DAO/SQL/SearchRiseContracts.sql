DECLARE @Contract VARCHAR(50) = '{Contract}'
DECLARE @Client VARCHAR(50) = '{Client}'
DECLARE @Status VARCHAR(10) = '{Status}'
DECLARE @StartDate VARCHAR(20) = '{StartDate}'
DECLARE @EndDate VARCHAR(20) = '{EndDate}'
DECLARE @SQLStatement VARCHAR(MAX) = ''

SET @SQLStatement = 'SELECT T0.U_DocEntry AS DocEntry, T1.DocNum, T1.CardCode, T1.CardName, SUM(ISNULL(T2.Quantity, 0)) AS HrsFeet, 
SUM(ISNULL(T6.Quantity, 0)) AS ExtrasInv, T1.DocTotal + ISNULL(T3.TotalInv, 0) AS ImportInv, 
ISNULL(T4.RealHrs, 0) AS RealHrs, SUM(ISNULL(T2.Quantity, 0)) - ISNULL(T4.RealHrs, 0) AS Difference, 
ISNULL(T4.Status, 0) AS ''Close''
FROM [@UG_TBL_MQ_RIDC] T0
INNER JOIN ORDR T1
	ON T1.DocEntry = T0.U_DocEntry
INNER JOIN RDR1 T2
	ON T1.DocEntry = T2.DocEntry
LEFT JOIN (SELECT NumAtCard, CASE 
	WHEN DocCur = ''MXP'' 
		THEN SUM(ISNULL(DocTotal, 0))
	ELSE SUM(ISNULL(DocTotalFC, 0))
END AS TotalInv
FROM OINV GROUP BY DocCur, NumAtCard) T3
	ON CAST(T1.DocNum AS varchar) = T3.NumAtCard
LEFT JOIN (SELECT 
	U_DocEntry, SUM(ISNULL(U_HrFeet, 0)) AS RealHrs, 
	CASE 
		WHEN SUM(ISNULL(U_Close, 0)) <= 0
			THEN 0
		ELSE 1
	END AS Status
FROM [@UG_MQ_RIHR] GROUP BY U_DocEntry) T4
	ON T1.DocEntry = t4.U_DocEntry
LEFT JOIN (SELECT NumAtCard, COUNT(*) AS ''Count'', SUM(ISNULL(INV1.Quantity, 0)) AS ''Quantity'' FROM OINV INNER JOIN INV1 ON INV1.DocEntry = OINV.DocEntry GROUP BY NumAtCard) T6
	ON CAST(T1.DocNum AS varchar) = T6.NumAtCard'

IF @Contract > 0 BEGIN
	SET @SQLStatement = @SQLStatement + ' AND T1.DocNum = ' + ''''+ @Contract +''''
END

IF @Client != '' BEGIN
	SET @SQLStatement = @SQLStatement + ' AND T1.CardCode = ' + ''''+ @Client +''''
END

IF @Status != '' BEGIN
	SET @SQLStatement = @SQLStatement + ' AND ISNULL(T4.Status, 0) = ' + ''''+ @Status +''''
END

IF @StartDate != '' AND @EndDate != ''
BEGIN
	SET @SQLStatement = @SQLStatement + ' AND T1.DocDate BETWEEN ' + ''''+ @StartDate +'''' + ' AND ' + ''''+ @EndDate +''''
END

IF @StartDate != '' AND @EndDate = ''
BEGIN
	SET @SQLStatement = @SQLStatement + ' AND T1.DocDate =' + ''''+ @StartDate +''''
END

IF @StartDate = '' AND @EndDate != ''
BEGIN
	SET @SQLStatement = @SQLStatement + ' AND T1.DocDate =' + ''''+ @EndDate +''''
END

SET @SQLStatement = @SQLStatement + ' GROUP BY T0.U_DocEntry, T1.DocNum, T1.CardCode, T1.CardName, T1.DocTotal, T3.TotalInv, T4.RealHrs, T4.Status, T6.Count'

print(@SQLStatement)

EXEC(@SQLStatement)