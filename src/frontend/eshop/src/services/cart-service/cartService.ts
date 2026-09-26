import { apiService } from "../api-service/apiService.ts";
import type {CartResponse} from "../../models/cart/responses/CartResponse.ts";

export const cartService = {
    get: () =>
        apiService.get<CartResponse>("cart"),

    addItem: (productId: string, quantity: number) =>
        apiService.post<CartResponse>("cart/items", {
            productId,
            quantity,
        }),
};