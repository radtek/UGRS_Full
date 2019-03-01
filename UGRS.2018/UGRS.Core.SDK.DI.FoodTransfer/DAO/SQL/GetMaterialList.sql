SELECT 
	  T0.ChildNum                                                              [LineNum],
	  CASE WHEN T3.VisResCode IS NULL THEN T0.Code ELSE T3.VisResCode  END     [Item],
	  CASE WHEN T3.VisResCode IS NULL THEN  T1.ItemName ELSE T3.ResName  END   [Desc], 
	  '{Whs}'                                                                  [Whs],
	  CASE WHEN T3.VisResCode IS NULL THEN  T4.OnHand ELSE 0 END               [Exist], 
	  T0.Quantity                                                              [Plan], 
	  T0.Quantity                                                              [Qty],
	  0                                                                        [Bags],
	  0                                                                        [Consumed],
	  ISNULL(T2.WipAcct, '')                                                   [AccCode],
	   ''                                                  [Method],
	   CASE WHEN T1.QryGroup27 = 'Y' THEN 1 ELSE 0 END                         [Prod],
	   CASE WHEN T1.QryGroup24 = 'Y' THEN 1 ELSE 0 END                         [BagsRequired],
	   CASE WHEN T3.VisResCode IS NULL THEN 0 ELSE 1 END                       [Resource],
	   CASE WHEN T1.InvntItem = 'Y' THEN 0 ELSE 1 END                          [Inventorial]
FROM 
	ITT1 T0
	LEFT JOIN (SELECT ItemCode, QryGroup27, QryGroup24, ItemName, OnHand, InvntItem FROM OITM) T1 ON T1.ItemCode = T0.Code
	LEFT JOIN (SELECT ItemCode,OnHand, WhsCode FROM OITW) T4 ON T4.ItemCode = T0.Code AND T4.WhsCode = '{Whs}'
	LEFT JOIN (SELECT VisResCode, QryGroup27, QryGroup24, ResName, 0 OnHand  FROM ORSC) T3 ON T3.VisResCode = T0.Code
	LEFT JOIN (SELECT 
					T0.WipAcct, 
					T2.ItemCode 
				FROM 
					OGAR T0 
					LEFT JOIN OSTC t1 on t1.Code = t0.VatGroup
					LEFT JOIN OITM t2 on t2.ItmsGrpCod = t0.ItmsGrpCod
				WHERE
					T0.WhsCode= '{Whs}' AND T1.Code IS NULL AND T0."Year" = YEAR(GETDATE())
					) T2 ON T2.ItemCode = T0.Code 
WHERE 
	t0.Father = '{Param}'