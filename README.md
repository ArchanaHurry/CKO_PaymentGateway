# Payment Gateway

An API based application built with .NET Core 2.2 and with a simulating acquiring Bank.
This API will allow a merchant to offer a way for their shoppers to pay for their items.

The Payment Gateway - Acts as a middleware between merchant and bank.

	1. Provide an Endpoint to accept payments/refunds and query processed transactions.
	2. Storing card information and forwarding payment requests and accepting payment to and from acquiring bank.

The Bank Mock API: 

	1. Returns a Unique identifier and a status that indicates whether the payment was successful.
	
# Assumptions/Constraints
	1. Payment/Refund only have 2 states: Success or Failure.
	2. Merchant sends his own bank account and the customer's payment card details for the transaction.
	3. The logic of using Payment Card Number to determine the bank associated to then forward the transaction to it has not been implemented.
	
# The Project Main Solution Breakdown:

# 1. Databases/Tables
	> Auth_Server:
		Store the user merchant credentials.
	> MerchantUsername
		All the usernames for each merchant is listed.
	> Transaction
		Payment and refund transactions passing through the gateway are stored.

NOTE:
All these databases have been created in Microsoft SQL Server. Please do use the scripts found in the project (PaymentGateway/Script) to recreate the tables and then modify the ConnectionStrings in the appsettings.json file.

# 2. PaymentGateway 
		- Exposes 3 Endpoints:
			1. POST ~/auth/token
				Request access token.
				=> Header:
					Key: Content-Type, Value: application/x-www-form-urlencoded
				=> Body: 
				   Key: grant_type, Value: password
				   Key: username, Value: "username@test.com"
				   Key: password, Value: "xxxxxx"
				   
			2. POST ~/Payment
				Payments are performed using this endpoint.
				Returns whether payment was processed/failed and gateway transaction ID.
				
				=> Header:
					Key: Authorization, Value: Bearer + "access token generated"
					Key: Cotnent-Type, Value: application/json
				=> Body: 
					{
						"MerchantBankAccount": 1234567952364125,
						"CardNumber": 4824342341435345,
						"ExpMonth": 12,
						"ExpYear": 2020,
						"CVV": 237,
						"Amount": 100.12,
						"Currency": "USD"
					}

			3. POST ~/Refund
				Refunds are performed using this endpoint.
				Returns whether refund was processed/failed and gateway transaction ID.
				
				=> Header:
					Key: Authorization, Value: Bearer + "access token generated"
					Key: Cotnent-Type, Value: application/json
				=> Body: 
					{
						"MerchantBankAccount": 1234567952364125,
						"CardNumber": 4824342341435345,
						"ExpMonth": 12,
						"ExpYear": 2020,
						"CVV": 237,
						"Amount": 100.12,
						"Currency": "USD"
					}
					
			4. GET ~/GetTransaction?id=
				Endpoint allows the user to search the transaction details stored in the gateway database using the 					gateway transaction ID.
				
				=> Header:
					Key: Authorization, Value: Bearer + "access token generated"
					
				=> Response:
					{
						"transactionId": "bdeed04c-384b-468f-c40e-08d7309304e6",
						"cardNumber": "2624 XXXX XXXX 4125 ",
						"expMonth": 10,
						"expYear": 2019,
						"cvv": 23,
						"amount": 1000000,
						"currency": "MUR",
						"transactionType": "Refund",
						"dateTime": "03/09/2019 17:24:54",
						"status": "Processed"
					}
					
# 3. BankMockAPI
		- Expose 2 APIs, for Payment and Refund. It returns a unique identifier that returns an OK response.
	
				=> Header:
					Key: Cotnent-Type, Value: application/json
				=> Body: 
					{
					    PayeeBankAccount = 1234567952364125 ,
					    PayerCardNumber  = 482434234143534,
					    ExpMonth         = 10,
					    ExpYear          = 2036,
					    CVV              = 952,
					    Amount           = 35000,
					    Currency         = "MUR" 
					}
