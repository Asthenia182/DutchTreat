import { __decorate } from "tslib";
import { Component } from '@angular/core';
let AppComponent = class AppComponent {
    constructor() {
        this.title = 'DutchTreat';
    }
};
AppComponent = __decorate([
    Component({
        selector: 'the-shop',
        template: `
    
    <div style="text-align:center" class="content">
      <h1>
        Welcome to {{title}}!
      </h1>
    </div>

    <router-outlet></router-outlet>
  `,
        styles: []
    })
], AppComponent);
export { AppComponent };
//# sourceMappingURL=app.component.js.map