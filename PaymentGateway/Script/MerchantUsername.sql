CREATE TABLE [MerchantUsername] (
    [Id] int NOT NULL IDENTITY,
    [Merchant] nvarchar(max) NULL,
    [Username] nvarchar(max) NULL,
    CONSTRAINT [PK_MerchantUsername] PRIMARY KEY ([Id])
);

GO