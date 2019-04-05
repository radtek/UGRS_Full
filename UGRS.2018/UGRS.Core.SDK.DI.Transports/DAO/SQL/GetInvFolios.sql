select U_GLO_Ticket, DocNum, Status, Type
from (

SELECT TOP 10 
U_GLO_Ticket, 
DocNum,
CASE WHEN DocStatus = 'O' THEN 'Abierto' ELSE 'Cerrado' END AS Status,
'FE' as Type
FROM OINV with (Nolock)
WHERE U_GLO_Ticket is not null and U_TR_Shared = 'Y'
AND U_GLO_Ticket = COALESCE(NULLIF('',''),U_GLO_Ticket) 
AND ( U_GLO_Ticket like '%%' or DocNum like '%%' )
order by DocEntry desc ) first
union all

select U_GLO_Ticket, DocNum, Status, Type from(
select Top 10 U_Ticket as U_GLO_Ticket, U_InternalFolio as DocNum, '' as Status, 'FI' as Type from [dbo].[@UG_TR_INTLFRGHT] with (Nolock) where U_Shared = 2 order by U_InternalFolio desc ) last
