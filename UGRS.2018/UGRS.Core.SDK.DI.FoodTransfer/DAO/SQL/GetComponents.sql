SELECT 
    T0.LineNum                                                                   [LineNum],
	CASE WHEN T2.VisResCode IS NULL THEN T0.ItemCode ELSE T2.VisResCode  END     [Item],
	CASE WHEN T2.VisResCode IS NULL THEN  T1.ItemName ELSE T2.ResName  END       [Desc], 
	T0.Warehouse                                                                 [Whs],
	CASE WHEN T2.VisResCode IS NULL THEN  T5.OnHand ELSE 0 END                   [Exist], 
	T0.PlannedQty                                                                [Plan], 
	T0.PlannedQty                                                                [Qty],
	ISNULL(T4.U_GLO_BagsBales, 0)                                                [Bags],
	T0.IssuedQty                                                                 [Consumed],
	ISNULL(T3.WipAcct, '')                                                       [AccCode],
    ISNULL(T0.IssueType, '')                                                     [Method],
	CASE WHEN T1.QryGroup27 = 'Y' THEN 1 ELSE 0 END                              [Prod],
	CASE WHEN T1.QryGroup24 = 'Y' THEN 1 ELSE 0 END                              [BagsRequired],
	CASE WHEN T2.VisResCode IS NULL THEN 0 ELSE 1 END                            [Resource],
	CASE WHEN T1.InvntItem = 'Y' THEN 0 ELSE 1 END                               [Inventorial]
FROM 
	WOR1 T0
	LEFT JOIN (SELECT ItemCode, QryGroup27, QryGroup24, ItemName, OnHand, InvntItem FROM OITM) T1 ON T1.ItemCode = T0.ItemCode
	LEFT JOIN (SELECT VisResCode, QryGroup27, QryGroup24, ResName, 0 OnHand  FROM ORSC) T2 ON T2.VisResCode = T0.ItemCode
	LEFT JOIN (SELECT ItemCode,OnHand, WhsCode FROM OITW) T5 ON T5.ItemCode = T1.ItemCode AND T5.WhsCode = T0.wareHouse
	LEFT JOIN (SELECT TOP 1 
						A0.WipAcct,
						A0.WhsCode
				FROM 
					OGAR A0 
				WHERE
					A0."Year" = YEAR(GETDATE()) AND ISNULL(A0.WipAcct,'') <> '') T3 ON T3.WhsCode = T0.Warehouse 
	LEFT JOIN (SELECT 
					T1.U_GLO_BagsBales,
					T2.ItemCode,
					T2.DocEntry
			   FROM 
					(SELECT DocEntry, U_GLO_DocNumSal FROM OWOR) T0
					INNER JOIN (SELECT DocEntry, ItemCode FROM WOR1) T2 ON T2.DocEntry = T0.DocEntry
					INNER JOIN (SELECT T1.U_GLO_BagsBales, T1.DocEntry, T0.DocNum, ItemCode 
								FROM OIGE T0 
								INNER JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry) T1 ON T0.U_GLO_DocNumSal = T1.DocNum AND T1.ItemCode = T2.ItemCode
					) T4 
				ON T4.DocEntry = T0.DocEntry AND T4.ItemCode = T0.ItemCode
WHERE 
	T0.DocEntry = '{Param}'