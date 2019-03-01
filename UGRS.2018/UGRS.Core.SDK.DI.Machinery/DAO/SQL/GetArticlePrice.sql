SELECT ISNULL(T1.Price, 0) AS Price FROM OPLN T0 
INNER JOIN ITM1 T1 
	ON T0.ListNum = T1.PriceList 
INNER JOIN [@UG_MQ_TYCN] T2 
	ON T2.U_ItemCode = T1.ItemCode
WHERE T0.U_GLO_Location ='MQHE' AND T2.U_ItemCode = '{ArticleCode}'