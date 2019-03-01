SELECT 
    T1.LineNum                [LineNum],
	T1.ItemCode               [Item],
	T2.ItemName               [Desc], 
	T1.WhsCode                [Whs],
	T4.OnHand                 [Exist], 
	T1.Quantity               [Plan], 
	0                         [Qty],
	ISNULL(U_GLO_BagsBales, 0)[Bags],
	T1.Quantity               [Consumed],
	ISNULL(T3.WipAcct, '')    [AccCode],
    ''                        [Method],
	CASE WHEN T2.QryGroup27 = 'Y' THEN 1 ELSE 0 END [Prod],
	CASE WHEN T2.QryGroup24 = 'Y' THEN 1 ELSE 0 END [BagsRequired],
	0                                               [Resource],
    CASE WHEN T2.InvntItem = 'Y' THEN 0 ELSE 1 END  [Inventorial]
FROM 
	OIGE T0
	LEFT JOIN IGE1 T1 ON T0.DocEntry = T1.DocEntry
	LEFT JOIN (SELECT ItemCode, QryGroup27, QryGroup24, ItemName, OnHand, InvntItem FROM OITM) T2 ON T2.ItemCode = T1.ItemCode
	LEFT JOIN (SELECT ItemCode,OnHand, WhsCode FROM OITW) T4 ON T4.ItemCode = T1.ItemCode AND T4.WhsCode = T1.WhsCode
	LEFT JOIN (SELECT TOP 1 
						A0.WipAcct,
						A0.WhsCode
				FROM 
					OGAR A0 
				WHERE
					A0."Year" = YEAR(GETDATE()) AND ISNULL(A0.WipAcct,'') <> '') T3 ON T3.WhsCode = T1.WhsCode 
WHERE 
	T0.DocNum = '{Param}'
