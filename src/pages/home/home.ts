import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ServiceNowProvider } from "../../providers/service-now/service-now";

@Component({
  selector: 'page-home',
  templateUrl: 'home.html',
  providers: [ServiceNowProvider]
})
export class HomePage {

  private objects: any;

  constructor(public navCtrl: NavController, private serviceNowApi: ServiceNowProvider) {

  }

  get() {
    this.objects = this.serviceNowApi.getIncidents().subscribe((data) => {
      JSON.parse(data)
      this.objects = data;
    })
  }
}
