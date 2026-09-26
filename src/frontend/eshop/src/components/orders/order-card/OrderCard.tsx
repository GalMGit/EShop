import { useNavigate } from "react-router-dom";
import type { OrderResponse } from "../../../models/orders/responses/OrderResponse.ts";
import "./OrderCard.css";

type OrderCardProps = {
    order: OrderResponse;
};

export const OrderCard = ({ order }: OrderCardProps) => {
    const navigate = useNavigate();

    const formattedDate = new Date(
        order.createdAt
    ).toLocaleDateString("en-US", {
        day: "numeric",
        month: "long",
        year: "numeric",
    });

    const handleClick = () => {
        navigate(`/orders/${order.id}`);
    };

    return (
        <article
            className="order-card"
            onClick={handleClick}
            role="button"
            tabIndex={0}
            onKeyDown={(event) => {
                if (event.key === "Enter" || event.key === " ") {
                    handleClick();
                }
            }}
        >
            <div className="order-card-main">
                <div className="order-card-info">
                    <span className="order-card-label">
                        ORDER
                    </span>

                    <h2>
                        #{order.id.slice(0, 8)}
                    </h2>

                    <span className="order-card-date">
                        {formattedDate}
                    </span>
                </div>

                <div className="order-card-status">
                    <span className="order-card-label">
                        STATUS
                    </span>

                    <span
                        className={`order-status order-status-${order.status.toLowerCase()}`}
                    >
                        {order.status}
                    </span>
                </div>

                <div className="order-card-total">
                    <span className="order-card-label">
                        TOTAL
                    </span>

                    <strong>
                        {order.totalAmount.toFixed(2)}Р
                    </strong>
                </div>
            </div>
        </article>
    );
};