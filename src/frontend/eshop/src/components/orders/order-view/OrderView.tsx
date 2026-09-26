import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { OrderWithItemsResponse } from "../../../models/orders/responses/OrderWithItemsResponse.ts";
import { orderService } from "../../../services/order-service/orderService.ts";
import "./OrderView.css";

export const OrderView = () => {
    const { orderId } = useParams();
    const navigate = useNavigate();

    const [order, setOrder] =
        useState<OrderWithItemsResponse | null>(null);

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!orderId) {
            setError("Order not found.");
            setLoading(false);
            return;
        }

        const loadOrder = async () => {
            try {
                setLoading(true);
                setError(null);

                const response =
                    await orderService.getById(orderId);

                setOrder(response.data);
            } catch (error) {
                console.error(error);
                setError("Failed to load order.");
            } finally {
                setLoading(false);
            }
        };

        loadOrder();
    }, [orderId]);

    if (loading) {
        return (
            <main className="order-view">
                <div className="order-view-status">
                    Loading order...
                </div>
            </main>
        );
    }

    if (error || !order) {
        return (
            <main className="order-view">
                <div className="order-view-status">
                    <p>
                        {error ?? "Order not found."}
                    </p>

                    <button
                        type="button"
                        onClick={() => navigate("/orders")}
                    >
                        Back to orders
                    </button>
                </div>
            </main>
        );
    }

    const formattedDate = new Date(
        order.createdAt
    ).toLocaleDateString("en-US", {
        day: "numeric",
        month: "long",
        year: "numeric",
    });

    return (
        <main className="order-view">
            <button
                className="order-back-button"
                type="button"
                onClick={() => navigate("/orders")}
            >
                ← Back to orders
            </button>

            <header className="order-view-header">
                <div>
                    <span className="order-view-label">
                        ORDER
                    </span>

                    <h1>
                        #{order.id.slice(0, 8)}
                    </h1>

                    <p>
                        {formattedDate}
                    </p>
                </div>

                <span
                    className={`order-view-status-badge order-status-${order.status.toLowerCase()}`}
                >
                    {order.status}
                </span>
            </header>

            <section className="order-view-items">
                <div className="order-view-section-header">
                    <span>
                        ORDER ITEMS
                    </span>

                    <strong>
                        {order.items.length}{" "}
                        {order.items.length === 1
                            ? "item"
                            : "items"}
                    </strong>
                </div>

                <div className="order-items-list">
                    {order.items.map((item) => (
                        <article
                            className="order-item"
                            key={item.id}
                        >
                            <div className="order-item-image">
                                {item.productImageUrl ? (
                                    <img
                                        src={item.productImageUrl}
                                        alt={item.productName}
                                    />
                                ) : (
                                    <div className="order-item-placeholder">
                                        No image
                                    </div>
                                )}
                            </div>

                            <div className="order-item-info">
                                <h2>
                                    {item.productName}
                                </h2>

                                <span>
                                    {item.unitPrice.toFixed(2)}Р
                                    {" × "}
                                    {item.quantity}
                                </span>
                            </div>

                            <strong className="order-item-total">
                                {item.totalPrice.toFixed(2)}Р
                            </strong>
                        </article>
                    ))}
                </div>
            </section>

            <aside className="order-view-summary">
                <span className="order-view-summary-label">
                    ORDER SUMMARY
                </span>

                <div className="order-view-summary-row">
                    <span>Items</span>

                    <strong>
                        {order.items.length}
                    </strong>
                </div>

                <div className="order-view-summary-divider" />

                <div className="order-view-summary-total">
                    <span>Total</span>

                    <strong>
                        {order.totalAmount.toFixed(2)}Р
                    </strong>
                </div>
            </aside>
        </main>
    );
};