SELECT 
	DocNum 
FROM 
	OWOR 
WHERE 
	"Status" = 'L' AND "Type" = 'S' AND DocNum NOT IN(SELECT U_MQ_OrigenFol FROM OIGE WHERE U_GLO_Status = 'C' AND U_MQ_OrigenFol IS NOT NULL AND Series = '{Series}' )

