var form = $('#the-form')
form.hide()

var buyButton = $('#buy-button')
buyButton.on('click', function () {
    console.log('Buying item')
})

var productInfo = $('.product-info li')
productInfo.on('click', function () {
    console.log('You clicked on ' + $(this).text())
})