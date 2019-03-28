SELECT T1.TransId FROM OJDT T1 with (nolock) 
full outer join OJDT T2 with (Nolock) on T2.StornoToTr = T1.TransId 
WHERE T1.ref1 = '{Folio}' and T1.TransCode = '{TransCode}'
and T1.AutoStorno='N' and T2.TransId is null 