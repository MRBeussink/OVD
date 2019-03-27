import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: () => any) {
  alertify.confirm(message, function(response) {
    console.log(response);
    if (response) {
      return okCallback();
    }
  });
}

newGroup() {
  alertify.genericDialog('genericDialog', function() {
    return {
        main: function(content) {
            this.setContent(content);
        },
        setup: function() {
            return {
                focus: {
                    element: function() {
                        return this.elements.body.querySelector(this.get('selector'));
                    },
                    select: true
                },
                options: {
                    basic: true,
                    maximizable: false,
                    resizable: false,
                    padding: false
                }
            };
        },
        settings: {
            selector: undefined
        }
    };
});
}

}
