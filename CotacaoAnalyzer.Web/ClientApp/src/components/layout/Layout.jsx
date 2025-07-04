import React from 'react';
import Header from './Header';
import MainContent from './MainContent';
import FlashMessage from '../ui/FlashMessage';

const Layout = ({ children }) => {
    return (
        <>
            <Header />
            <MainContent>
                <FlashMessage />
                {children}
            </MainContent>
        </>
    );
};

export default Layout;
