import type {CartItemResponse} from "./CartItemResponse.ts";

export type CartResponse = {
    id: string;
    userId: string;
    items: CartItemResponse[],
    total: number;
}