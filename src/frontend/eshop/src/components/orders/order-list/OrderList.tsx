import { useEffect, useState } from "react";
import type { OrderResponse } from "../../../models/orders/responses/OrderResponse.ts";
import { orderService } from "../../../services/order-service/orderService.ts";
import { OrderCard } from "../order-card/OrderCard.tsx";
import "./OrderList.css";

export const OrderList = () => {
    const [orders, setOrders] = useState<OrderResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadOrders = async () => {
            try {
                setLoading(true);
                setError(null);

                const response = await orderService.getAll();

                setOrders(response.data);
            } catch (error) {
                console.error(error);
                setError("Failed to load orders.");
            } finally {
                setLoading(false);
            }
        };

        loadOrders();
    }, []);

    if (loading) {
        return (
            <div className="order-list-status">
                Loading orders...
            </div>
        );
    }

    if (error) {
        return (
            <div className="order-list-status order-list-error">
                {error}
            </div>
        );
    }

    if (orders.length === 0) {
        return (
            <div className="order-list-empty">
                <h2>No orders yet</h2>

                <p>
                    Your completed orders will appear here.
                </p>
            </div>
        );
    }

    return (
        <div className="order-list">
            {orders.map((order) => (
                <OrderCard
                    key={order.id}
                    order={order}
                />
            ))}
        </div>
    );
};