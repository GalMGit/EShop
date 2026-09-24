export type CartItemResponse = {
    id: string;
    cartId: string;
    productName: string;
    thumbnailUrl?: string;
    productId: string;
    unitPrice: number;
    totalPrice: number;
    quantity: number;
}