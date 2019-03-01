SELECT 
	B0.CardCode [Code],
	B0.CardName [Name],
	B0.WhsCode  [Whs], 
	B0.ItemName [Type],
	B0.Quantity [Exist], 
	0           [Food],
	0           [Bags], 
	CASE WHEN B1.U_GLO_Corral IS NULL THEN 'NO' ELSE 'SI' END [Deliv] 
FROM
	(SELECT  T0.ItemCode ,t0.WhsCode, Sum(T0.Quantity) as Quantity, T5.CardCode, T5.CardName,T6.ItemName
  FROM  dbo.OBTQ T0  
  INNER JOIN OBTN T1 ON T0.SysNumber=T1.SysNumber and t0.ItemCode= t1.ItemCode
  INNER JOIN OWHS T2 ON T2.WhsCode=T0.WhsCode
  INNER JOIN OLCT T3 ON T3.Code = T2.Location
  INNER JOIN OCRD T5 ON T5.CardCode = T1.MnfSerial
  INNER JOIN OITM T6 ON T6.ItemCode = T0.ItemCode WHERE t3.Location = '{Location}' and T0.Quantity > 0
  GROUP BY t0.WhsCode, t0.ItemCode, t0.Quantity, t5.CardCode, t5.CardName, t6.ItemName
  ) B0
  LEFT JOIN (SELECT t1.U_GLO_Corral, CardCode From ODLN T0
	INNER JOIN DLN1 T1 ON t0.docentry= t1.docentry
	INNER JOIN OWHS T2 ON t2.whscode= t1.U_GLO_Corral
	INNER JOIN OLCT T3 ON t2.location = t3.code
	WHERE 
		t3.Location= '{Location}' and T0.CANCELED = 'N' and t0.docdate ='{InDate}'
	GROUP BY 
		t1.U_GLO_Corral, CardCode) B1 ON B1.U_GLO_Corral= B0.WhsCode AND B1.CardCode = B0.CardCode