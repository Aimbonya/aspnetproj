using _253501_mammadov.Extensions;
using mammadov.Domain.Entities;

public class SessionCart : Cart
{
    private ISession _session;

    public SessionCart() { }

    public SessionCart(ISession session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    private void SaveToSession()
    {
        if (_session == null)
        {
            throw new InvalidOperationException("Session is not initialized.");
        }
        _session.SetObject("cart", this);
    }

    public static Cart GetCartFromSession(ISession session)
    {
        var cart = session.GetObject<SessionCart>("cart");
        if (cart == null)
        {
            cart = new SessionCart(session);
        }
        cart._session = session; // Обновляем _session
        return cart;
    }

    public override void AddToCart(Fruit product)
    {
        base.AddToCart(product);
        SaveToSession();
    }

    public override void RemoveItem(int productId)
    {
        base.RemoveItem(productId);
        SaveToSession();
    }

    public override void ClearAll()
    {
        base.ClearAll();
        SaveToSession();
    }
}
