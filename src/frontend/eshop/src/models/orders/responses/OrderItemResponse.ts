export type OrderItemResponse = {
    id: string;
    productId: string;
    productName: string;
    productImageUrl?: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
};