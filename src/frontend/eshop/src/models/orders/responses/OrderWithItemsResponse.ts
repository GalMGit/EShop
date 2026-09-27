import type { OrderItemResponse } from "./OrderItemResponse.ts";

export type OrderWithItemsResponse = {
    id: string;
    totalAmount: number;
    status: string;
    createdAt: string;
    cancellationReason?: string;
    items: OrderItemResponse[];
};