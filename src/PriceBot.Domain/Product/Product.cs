﻿namespace PriceBot.Domain.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BRLValue { get; set; }
        public decimal USDValue { get; set; }
        public decimal EURValue { get; set; }
    }
}