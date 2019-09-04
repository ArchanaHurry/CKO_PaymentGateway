CREATE TABLE [Transaction] (
    [Id] uniqueidentifier NOT NULL DEFAULT NEWID(),
    [Merchant] nvarchar(max) NULL,
	[MerchantBankAccount] bigint NOT NULL,
    [CardNumber] bigint NOT NULL,
    [ExpMonth] int NOT NULL,
    [ExpYear] int NOT NULL,
    [CVV] int NOT NULL,
    [Amount] float NOT NULL,
    [Currency] nvarchar(max) NULL,
    [TransactionType] nvarchar(max) NULL,
    [BankTransactionId] nvarchar(max) NULL,
    [BankResponseStatus] nvarchar(max) NULL,
    [DateTime] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id])
);

GO