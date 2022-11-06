import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EmailSettings } from '../Types/email-settings';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css'],
})
export class ContactComponent {

  emailSettings: EmailSettings = {
    FromName: "",
    ToEmail: "",
    Subject: "",
    EmailBody: "",
    FromEmail: ""
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  onSubmit(email: any) {

    this.emailSettings = email.value as EmailSettings;
    this.emailSettings.ToEmail = "postmaster@remoteservicesllc.com";

    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }

    this.http.post<boolean>(this.baseUrl + 'email/send', email.value, httpOptions).subscribe(result => {
    }, error => console.error(error));
  }
}
