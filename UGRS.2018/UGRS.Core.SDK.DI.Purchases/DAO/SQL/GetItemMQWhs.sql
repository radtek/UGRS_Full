 select ItemCode from OITM with (Nolock) where ItemCode = '{ItemCode}' 
		   and(QryGroup23 = 'Y'
		   or QryGroup26 = 'Y'
		   or QryGroup28 = 'Y'
		   or QryGroup29 = 'Y'
		   or QryGroup30 = 'Y')