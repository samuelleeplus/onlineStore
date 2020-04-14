CREATE TABLE [dbo].[OrderedProducts] (
    [ProductId] INT IDENTITY (1, 1) NOT NULL,
    [Quantity]  INT NOT NULL,
    [OrderId]   INT NOT NULL,
    CONSTRAINT [PK_OrderedProducts] PRIMARY KEY CLUSTERED ([ProductId] ASC),
    CONSTRAINT [FK_OrderedProducts_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_OrderedProducts_OrderId]
    ON [dbo].[OrderedProducts]([OrderId] ASC);

