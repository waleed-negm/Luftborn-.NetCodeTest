export interface Auth {
	userId: string;
	userName: string;
	email: string;
	roles: string[];
	token: string;
	expiresOn: Date;
	refreshToken: string;
	refreshTokenExpiration: Date;
}
