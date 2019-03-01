 select 
U_Prefix+U_EarringFrom as Desde,

(case when U_EarringTo != '' then 
U_Prefix+U_EarringTo else '' end) as Hasta,

Code,
U_Prefix as Prefi,
U_EarringFrom as EFrom,
U_EarringTo as ETo
from [@UG_PE_ERNK] where  U_BaseEntry = '{BaseEntry}' and U_Cancelled = 'N'



