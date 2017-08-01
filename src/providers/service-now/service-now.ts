import { Injectable } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from "rxjs/Observable";

@Injectable()
export class ServiceNowProvider {

  constructor(public http: Http) {

  }

  getIncidents():Observable<any> {
    
    let username: string = 'Admin';
    let password: string = '#Hacknow17';
    let headers = new Headers();
    headers.append("Authorization", "Basic " + btoa(username + ":" + password));

    RequestOptions
    
    this.http.get("https://hackathon178.service-now.com/api/now/v1/table/incident?sysparm_query=active=true^ORDERBYDESCsys_updated_on", new RequestOptions({headers: headers}))
    .map(res => res.json())
  }
}
