// создать в папке js новый js файл StripePayment.js
redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51KRBEHEaymVdK2Bl89SzKH21Vg3kowmcyyPDqPeMuZr5mk96H4Ms1tBGt5qF48sf7kaAHWA5GOFBF2tWwFa7HTqX00YsP42khp'); // <-- Сюда вставляем Stripe public API Key
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};

// После реализации скрипта не забываем его подключить в файл шаблона Index.html