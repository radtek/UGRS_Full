SELECT 
	  ItemCode        [Item],
	  (-TransValue)   [LineTotal] 
FROM 
	OINM T0 
WHERE  
	T0.TransType = '{ObjType}' AND T0.BASE_REF = (SELECT U_GLO_DocNumSal FROM OWOR WHERE DocEntry = '{DocEntry}') 

