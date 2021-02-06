import { __decorate } from "tslib";
import { Component } from "@angular/core";
let ProductList = class ProductList {
    constructor() {
        this.products = [{
                title: "First Product",
                price: 19.99
            },
            {
                title: "Second Product",
                price: 19.99
            },
            {
                title: "Third Product",
                price: 19.99
            }];
    }
};
ProductList = __decorate([
    Component({
        selector: "product-list",
        templateUrl: "productList.component.html",
        styleUrls: []
    })
], ProductList);
export { ProductList };
//# sourceMappingURL=productList.component.js.map