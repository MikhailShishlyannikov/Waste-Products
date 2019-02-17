// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  logLevel: 0,

  apiHostUrl: 'https://localhost:44326',
  // apiHostUrl: 'https://waste-api.belpyro.net',

  /* Identity */
  iderntityHostUrl: 'https://localhost:44326/identity',
  // iderntityHostUrl: 'https://waste-api.belpyro.net/identity',
  clientId: 'wasteproducts.front.angular',

  scope: 'openid profile email wasteproducts-api',
  dummyClientSecret: 'F0E56438-BCDE-401E-BDE5-303BA812186F',

};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
