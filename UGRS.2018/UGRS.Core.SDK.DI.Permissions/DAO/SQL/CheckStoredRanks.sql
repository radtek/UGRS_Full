
select * from [@UG_PE_ERNK] 
where (RIGHT(U_EarringFrom, LEN(U_EarringFrom) - 1) between cast('{EarringFrom}'as int) and cast('{EarringTo}' as int) and U_Cancelled ='N') 



