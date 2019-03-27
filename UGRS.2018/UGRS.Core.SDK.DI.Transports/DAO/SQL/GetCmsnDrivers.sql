--Empleado operador de transporte
	 select OHEM.empID, OSLP.SlpName,
	-- ISNULL((select U_Value from [@UG_CONFIG] with(nolock) where Name = 'TR_LIQ_IMPORTE'), 0)
	 OHEM.Salary as WkDisc
	 from OHEM with (Nolock)
	 inner join OHPS  with (Nolock) on OHEM.Position = OHPS.posID
	 inner join OUDP with (Nolock) on OHEM.dept = OUDP.Code
	inner join OSLP with (nolock) on OHEM.SalesPrson = OSLP.SlpCode
	 where OHPS.Name = 'OPERADOR' and OUDP.Name = 'TRANSPORTES' and OHEM.Active = 'Y'
