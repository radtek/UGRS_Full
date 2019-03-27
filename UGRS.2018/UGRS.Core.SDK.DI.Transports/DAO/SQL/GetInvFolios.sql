SELECT TOP 10 
U_GLO_Ticket, 
DocNum,
CASE WHEN DocStatus = 'O' THEN 'Abierto' ELSE 'Cerrado' END AS Status
FROM OINV
WHERE U_GLO_Ticket is not null
AND U_GLO_Ticket = COALESCE(NULLIF('{Folio}',''),U_GLO_Ticket) 
AND ( U_GLO_Ticket like '%{Search}%' or DocNum like '%{Search}%' )
order by DocEntry desc