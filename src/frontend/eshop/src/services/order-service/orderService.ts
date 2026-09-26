import { apiService } from "../api-service/apiService.ts";
import type {CheckoutResponse} from "../../models/orders/responses/CheckoutResponse.ts";
import type {OrderResponse} from "../../models/orders/responses/OrderResponse.ts";
import type {OrderWithItemsResponse} from "../../models/orders/responses/OrderWithItemsResponse.ts";

export const orderService = {
    checkout: () =>
        apiService.post<CheckoutResponse>("orders/checkout"),

    getAll: () =>
        apiService.get<OrderResponse[]>("orders"),

    getById: (orderId: string) =>
        apiService.get<OrderWithItemsResponse>(
            `orders/${orderId}`
        ),
};