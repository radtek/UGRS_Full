select CAST(U_GLO_Coments AS VARCHAR(MAX)) as U_GLO_Coments
from JDT1 
where U_SU_Folio = '{Folio}' and U_GLO_Auxiliar = '{Cardcode}' and U_GLO_Coments is not null 
group by  CAST(U_GLO_Coments AS VARCHAR(MAX))